using System;

namespace ViertelStdToolLib.Converter
{
    public class ContractValidityConverter : IContractValidityConverter
    {
        #region Convert contract data to validity delivery start time.
        /// <summary>
        /// Convert contract data to validity delivery start time.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="deliveryTimeStart"></param>
        public void ConvertContractToValidityStartTime(string contract, ref string deliveryTimeStart)
        {
            int hour;
            int quarter;
            string hourString = string.Empty;
            string[] split = contract.Split('Q', 'q', '_');

            // Preset dateTime
            DateTime contractDeliveryStartTime = DateTime.Now; ; 

            if ((split.Length >= 2) && (split.Length <= 3))
            {
                // Hold in temp. string variable.
                hourString = split[0];

                if (hourString.StartsWith("T"))
                {
                    // If string starts with T (means Contract will start next day) - char 'T' will be removed. 
                    hourString = hourString.Remove(0, 1);
                }

                hour = Convert.ToInt16(hourString);
                 quarter = Convert.ToInt16(split[1]);
                if ((quarter >= 1) & (quarter <= 4))
                {
                    if ((hour >= 0) & (hour <= 23))
                    {
                        // Determine the Tud start time.  
                        switch (quarter)
                        {
                            case 1:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":00");
                                break;
                            case 2:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":15");
                                break;
                            case 3:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":30");
                                break;
                            case 4:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":45");
                                break;
                        }

                        // This is the start time of the contract as used in xml node.
                        deliveryTimeStart = contractDeliveryStartTime.ToString("HH:mm");                                          
                    }
                }
            }
        }
        #endregion

        #region Convert contract data to validity end time.
        /// <summary>
        /// Convert contract data to validity end time.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="deliveryTimeEnd"></param>
        public void ConvertContractToValidityEndTime(string contract, ref string deliveryTimeEnd)
        {
            int hour;
            int quarter;
            string hourString = string.Empty;
            string[] split = contract.Split('Q', 'q', '_');

            // Preset dateTime
            DateTime contractDeliveryStartTime = DateTime.Now; ;

            if ((split.Length >= 2) && (split.Length <= 3))
            {
                // Hold in temp. string variable.
                hourString = split[0];

                if (hourString.StartsWith("T"))
                {
                    // If string starts with T (means Contract will start next day) - char 'T' will be removed. 
                    hourString = hourString.Remove(0, 1);
                }

                hour = Convert.ToInt16(hourString);
                quarter = Convert.ToInt16(split[1]);
                if ((quarter >= 1) & (quarter <= 4))
                {
                    if ((hour >= 0) & (hour <= 23))
                    {
                        // Determine the Tud start time.  
                        switch (quarter)
                        {
                            case 1:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":15");
                                break;
                            case 2:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":30");
                                break;
                            case 3:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":45");
                                break;
                            case 4:
                                contractDeliveryStartTime = Convert.ToDateTime(hour + ":00");
                                break;
                        }

                        // This is the start time of the contract as used in xml node.
                        deliveryTimeEnd = contractDeliveryStartTime.ToString("HH:mm");
                    }
                }
            }
        }
        #endregion
    }
}

