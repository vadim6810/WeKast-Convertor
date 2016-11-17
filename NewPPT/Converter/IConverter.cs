using System;
using System.IO;

namespace WeCastConvertor.Converter
{
    public interface IConverter
    {
        string Convert(String file);
    }
}