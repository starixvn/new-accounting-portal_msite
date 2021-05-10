using System;

namespace Fanex.DeleteTempVariable.Model
{
    public class Match
    {
        public int MatchId { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public string EventName
        {
            get
            {
                return $"{HomeTeam} - vs - {AwayTeam}";
            }
        }

        public DateTime CreatedDateTime { get; set; }

        public string SportName { get; set; }

        public string Action { get; set; }

        public string ErrorMsg { get; set; }

        public string ProductName
        {
            get
            {
                switch (KeyId)
                {
                    case 1:
                        return "Sportbook, Numbergame";

                    case 2:
                        return "Outright";

                    case 3:
                        return "Metric Gaming";

                    case 4:
                        return "VS Racing";

                    case 5:
                        return "BetBuilder";

                    case 6:
                        return "Fast Market";

                    case 7:
                        return "Fast Market SABA Time Machine";

                    default:
                        return string.Empty;
                }
            }
        }

        public int Value { get; set; }

        public int TicketCount { get; set; }

        public bool WrongBalance { get; set; }

        public int IsCalculating { get; set; }

        public string Status
        {
            get
            {
                switch (IsCalculating)
                {
                    case 1:
                        return "Running";

                    case 3:
                        return "Failed";

                    default:
                        return string.Empty;
                }
            }
        }

        public int KeyId { get; set; }

        public string MarketId { get; set; }

        public string Remark { get; set; }
    }
}