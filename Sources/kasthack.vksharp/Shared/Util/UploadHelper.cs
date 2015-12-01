﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using kasthack.vksharp.DataTypes.Entities;
using Newtonsoft.Json;

namespace kasthack.vksharp.Util {

    public class UploadHelper {

        private readonly Api _api;
        private readonly HttpClient _client;

        public UploadHelper( Api api ) {
            _api = api;
            _client = new HttpClient();
        }

        public async Task<IEnumerable<Photo>> UploadPhotos( long albumId, int? userId, params string[] files ) {
            var ret = new List<Photo>( files.Length );
            int? gid = 0;
            if ( userId != null && userId < 0 ) gid = -userId;
            foreach ( var fileg in files.Select( ( path, index ) => new { path, index } ).GroupBy( a => a.index / 5 ) ) {
                FileStream[] streams = null;
                try {
                    var us = await _api.Photos.GetUploadServer( albumId, gid == 0 ? null : gid ).ConfigureAwait( false );
                    var ul = fileg.Select( a => a.path ).ToArray();
                    streams = ul.Select( File.OpenRead ).ToArray();
                    VkPhotoUploadResponse pr;
                    using ( var content = new MultipartFormDataContent() ) {
                        for ( var index = 0; index < streams.Length; index++ )
                            content.Add( new StreamContent( streams[ index ] ), "file" + index, index + Path.GetExtension( ul[ index ] ) );
                        var upl = await _client.PostAsync( us.UploadUrl, content ).ConfigureAwait( false );
                        var respjson = await upl.Content.ReadAsStringAsync().ConfigureAwait( false );
                        pr = JsonConvert.DeserializeObject<VkPhotoUploadResponse>( respjson );
                    }
                    ret.AddRange( await _api.Photos.Save( (int) pr.aid, pr.server, pr.photos_list, pr.hash ).ConfigureAwait( false ) );
                }
                finally {
                    foreach ( var stream in streams )
                        try {
                            stream.Close();
                        }
                        catch {}
                }
            }
            return ret;
        }

        private class VkPhotoUploadResponse {

            // ReSharper disable InconsistentNaming
            public string server { get; set; }
            public string hash { get; set; }
            public long aid { get; set; }
            public string photos_list { get; set; }
            // ReSharper restore InconsistentNaming
        }

    }

}