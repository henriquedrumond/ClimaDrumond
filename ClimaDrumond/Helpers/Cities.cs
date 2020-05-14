﻿using System;
namespace ClimaDrumond.Helpers
{
    public class Cities
    {
        public int id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public Coord coord { get; set; }
    }

    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }
}
