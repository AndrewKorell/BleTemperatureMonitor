using BleTempMonitor.Models;
using Plugin.BLE.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Helpers
{

    public interface IBleHelpers
    {
        void ParseAdvertisementReord(AdvertisementRecord r, ref AdverstisementModel model);
    }

    public class BleHelper : IBleHelpers
    {
        public void ParseAdvertisementReord(AdvertisementRecord r, ref AdverstisementModel model)
        {
            switch ((byte)r.Type)
            {
                case (byte)AdvertisementRecordType.Flags:
                    model.Flags = r.Data;
                    break;

                case (byte)AdvertisementRecordType.ServiceData:
                    model.ServiceData = r.Data;
                    break;

                case (byte)AdvertisementRecordType.CompleteLocalName:
                    model.CompleteLocalNameData = r.Data;
                    break;

                case (byte)AdvertisementRecordType.TxPowerLevel:
                    model.TxPowerLevel = r.Data;
                    break;

                case (byte)AdvertisementRecordType.ManufacturerSpecificData:

                    model.ManufacturerSpecificData = r.Data;
                    break;

                    //todo: Add rest of AdvertisementRecordTypes to this function and model

            }
        }
    }
}
