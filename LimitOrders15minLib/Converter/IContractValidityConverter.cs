using System;

namespace ViertelStdToolLib.Converter
{
    public interface IContractValidityConverter
    {
        /// <summary>
        /// Convert contract data to validity delivery start time.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="deliveryTimeStart"></param>
        void ConvertContractToValidityEndTime(string contract, ref string deliveryTimeStart);
        /// <summary>
        /// Convert contract data to contrat delivery end time.
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="deliveryTimeEnd"></param>
        void ConvertContractToValidityStartTime(string contract, ref string deliveryTimeEnd);
    }
}