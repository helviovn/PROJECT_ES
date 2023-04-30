using System;

namespace PROJECT_ES.Data
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime data_fim { get; set; }
        public int n_participantes { get; set; }
    }
}
