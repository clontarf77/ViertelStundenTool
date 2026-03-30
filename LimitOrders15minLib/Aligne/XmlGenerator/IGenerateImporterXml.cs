using System.Collections.Generic;
using System.Xml;
using ViertelStdToolLib.Aligne.Importer.DealStructure;

namespace ViertelStdToolLib.Aligne.Importer.Generate.Xml
{
    public interface IGenerateImporterXml
    {
        /// <summary>
        /// Generate template XML for VSK for several trades.
        /// </summary>
        /// <param name="parameterList"></param>          
        void GenerateXmlForVsk(List<ParameterXmlGeneration> parameterList);       
    }
}