using D2MapApi.Common.Enumerations.GameData;

using Microsoft.Extensions.Logging;

namespace D2MapApi.Server.Grpc.Services.GameData.Edition;

internal class D2EditionService(ILogger<D2EditionService> i_logger)
{
    private Dictionary<string, D2GameEdition> GameEditions { get; } = new()
                                                                    {
                                                                        { "D5860C091B5764BBC60BA027CE3CBB18F0A9009DE078ED4785CD0CCE33824240", D2GameEdition.CLASSIC },
                                                                        { "B115FA675290EAB1E664BBB7AA469D5701EE75E86765210A2C030C4251A01F22", D2GameEdition.CLASSIC },
                                                                        { "EE1DD916A917A43C15361FE2B887AFF21C4DAF9FD8C8AF39E05318B0AB4099D7", D2GameEdition.CLASSIC },
                                                                        { "924FFDBDDDCC17599B1662BAD5D89009EC0D24DD0CEB5347C0BC727C0C56D1F6", D2GameEdition.CLASSIC },
                                                                        { "50FF5F0370A8E8632DDED10B68F81743330B120A7F2BBF977974A17E4047A97F", D2GameEdition.CLASSIC },
                                                                        { "A00B99DD9FA3E98B38EF0D44988AFB4E5781309E2CFCD27E81E438D4F61A2072", D2GameEdition.CLASSIC },
                                                                        { "DCC710132C650231181BC776FDA85DDC30928C6C2D38B919132C31E48F7B2A6D", D2GameEdition.CLASSIC },
                                                                        { "EFDA7B5D5D03262AEF0B661B1AA753625495B3B28BB2E3F1F13561BABDEEB308", D2GameEdition.CLASSIC },
                                                                        { "4712CD26668696D7D01B90B85D55B718601A6EECC95F0C9BAB1E16B3F5D6E59C", D2GameEdition.CLASSIC },
                                                                        { "CF5647D244F2655CA09778704A0068BD965872ADCD504DC43D519550F6ED5C8C", D2GameEdition.CLASSIC },
                                                                        { "FFA78A89636B03EBB8560907688193895D27E6736AAD44D0442E3207BA67AD44", D2GameEdition.CLASSIC },
                                                                        { "F669AD517F99438067AC8F10FD21939A697776B8A545D7E623D93FD0B1109542", D2GameEdition.CLASSIC },
                                                                        { "8671802CB23858D4A6F3BC030D9FC47603372A4C7596E3316748415DF5D5CD35", D2GameEdition.CLASSIC },
                                                                        { "8596865FFB58723CFE6E49076B8B1825285433CF240702A1623F22E5F983CA88", D2GameEdition.CLASSIC },
                                                                        { "D8A7BAE2CD3E14D72002E88DDE9B6DB630604E536AA2EFF6E2DB442CBB033907", D2GameEdition.CLASSIC },
                                                                        { "DFD1C61746C17EBA0DD3DAC206B1695CDBBF9FE22E270C786EE6DD799C4E2A3A", D2GameEdition.CLASSIC },
                                                                        { "96746BF46F2893F87FF0655514C54088D0114448BCF884358D818E5C59606F8F", D2GameEdition.CLASSIC },
                                                                        { "3CA642E872DBD9ABACB166DCA0CBF41C419ED1B1BA48339953117A01D05A270C", D2GameEdition.CLASSIC },
                                                                        { "4D863B672F3E16FCE6DD4E5EE795F4429195DB2635C4EACBEF37F435B69FEB5D", D2GameEdition.CLASSIC },
                                                                        { "9622CA993DE5F5738AD437CC9C4B75AD35E439EC9AAFF6A3501870861A5A0EF1", D2GameEdition.CLASSIC },
                                                                        { "306812BF7F37E21FECB77D11E23F28005C97EEB2C7DD666B1352C2BB1DE87CB5", D2GameEdition.CLASSIC },
                                                                        { "28FAC184096FC36A37C5126590E65E71A3C9569C18B9000357FD7613A56B8C3C", D2GameEdition.CLASSIC },
                                                                        { "6C7B8DFBB4092EC38066F2660D7E065E44E94D80258265A46201E1AF073A3173", D2GameEdition.CLASSIC },
                                                                        { "CBE413EDB4AF9495DB06D489715922D4DCDD77A91701CEF089A6B2979D453BBD", D2GameEdition.CLASSIC },
                                                                        { "DD899789710B768682CB063854932510C6116B503A829FE0632CFD5383E579E6", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "EE2DFF122FD2B373AE8CFF38ED48DDBD9032DDAD916A5D0A93F2B40F8E7B9217", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "4DAEEC8A5659EBA41CAF90837A4C62EE996289E7291FD82FE2F993C07C18B942", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "ADD36ECB630A3B7CD73700BC22298F340CE9999BA97BF172E9A034459FB44A88", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "B211B8AD79E9F2BBB29F6E7B0D861CADBD1FE63438A12D740F01404FD2CE8DF6", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "5BFA2D47D8D5521D8E6AAFE46FE6D331BF6EE4D1CBDB42D2D39C7FA682CA825F", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "517A0CE7EA79444AA2290249B34F7402C5E81DA767882C2286C548756B1C92C7", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "8CC53AA120D135FD0EAAABCB1500C1773D95B93E64FB6477ADE333E4EB819C94", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "45FDB5A87FC37C52103C56A0AA701C4F6C8B31DE0721AD535FE07E5958F5A962", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "821FB4FB617B440403A1E2B576139386F381369046E39DBE9BE4458A2439C952", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "9CB3D8DD0FFAC92C164BA2A29F358D17FB372380ABDCC15F871831F262DE09FA", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "74FE9C092A521F7710392548C82F81544E531107DB2358617E83818874DB40A2", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "6CA6F345C11F47EAF6DC629457EC2EA0020FA7FBA4AA9BB2248F911B66870DDD", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "C99E3067E6A7EEF7800680AC6BED2BEB1A50CC285933166BCD7FBE4F394446A6", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "0680E266B1FBBC8E7815B0A7C4F67267DE38FD9063FCEEAE2DA0306389A52178", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "AD13296AE56B921987F1088E22D01D4EDD4A56C0BD15AF186549DE87A03AF29C", D2GameEdition.LORD_OF_DESTRUCTION },
                                                                        { "631066C1649C4EA9FFE48BF97E24C00BCA1F7A6759C21150F1A79982589ADAAF", D2GameEdition.LORD_OF_DESTRUCTION }
                                                                    };

    internal D2GameEdition GetGameEdition(string p_hashString)
    {
        i_logger.LogDebug("Getting game edition for hash '{HashString}'", p_hashString);
        
        return GameEditions.GetValueOrDefault(p_hashString, D2GameEdition.UNKNOWN);
    }
}