﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToSic.Cre8Magic.Client.Menus.Settings
{
    internal class StartingPoint
    {
        public int Id { get; set; }

        public bool Force { get; set; } = false;

        public string From { get; set; } = "*"; // "*", ".", "43"

        public int Level { get; set; } = 0; // 0 meaning current, not top...// -1, -2, -3; 1, 2, 3

        public bool Children { get; set; } = false;

        [JsonIgnore] public string Debug => JsonSerializer.Serialize(this);
    }
}
