﻿namespace kasthack.vksharp.DataTypes.Entities {
    public class School {
        public string Class { get; set; }
        public string Name { get; set; }
        public string Speciality { get; set; }
        public int City { get; set; }
        public int Country { get; set; }
        public int Id { get; set; }
        public int Type { get; set; }
        public string TypeStr { get; set; }
        public ushort? YearGraduated { get; set; }
        public ushort? YearFrom { get; set; }

        public ushort? YearTo { get; set; }
    }
}
