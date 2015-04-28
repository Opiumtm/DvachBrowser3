using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DvachBrowser3.Configuration.Makaba;

namespace DvachBrowser3.Engines.Makaba
{
    /// <summary>
    /// Сервис коррекции постов.
    /// </summary>
    public sealed class MakabaPostCorrectionService : ServiceBase, IPostCorrectionService
    {
        private delegate bool ReplaceDelegate(ref string source);

        private readonly Func<string, string>[] correctors;

        private readonly ReplaceDelegate[] wakabaReplaces;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        public MakabaPostCorrectionService(IServiceProvider services) : base(services)
        {
            correctors = new Func<string, string>[]
                {
                    CorrectWakaba,
                    ApplySmileMarkup,
                    CorrectLines
                };
            wakabaReplaces = new ReplaceDelegate[]
                {
                    ReplaceMarkup("***", "[b][i]", "[/i][/b]"),
                    ReplaceMarkup("**", "[b]", "[/b]"),
                    ReplaceMarkup("*", "[i]", "[/i]"),
                    ReplaceMarkup("%%", "[spoiler]", "[/spoiler]"),
                    ReplaceMarkup("__", "[u]", "[/u]"),
                    ReplaceMarkup("`", "[code]", "[/code]"),
                    ReplaceAtLineStart("    ", "[code]", "[/code]"),
                    ReplaceStrike,
                };
            config = new MakabaPostConfig();
        }

        /// <summary>
        /// Конфигурация.
        /// </summary>
        public IConfiguration Configuration
        {
            get { return PostConfig; }
        }

        private readonly MakabaPostConfig config;

        /// <summary>
        /// Конфигурация.
        /// </summary>
        public IMakabaPostConfig PostConfig
        {
            get { return config; }
        }

        /// <summary>
        /// Корректировать текст поста.
        /// </summary>
        /// <param name="text">Текст до коррекции.</param>
        /// <returns>Текст после коррекции.</returns>
        public string CorrectPostText(string text)
        {
            return correctors.Aggregate(text, (current, corrector) => corrector(current));
        }

        private string CorrectWakaba(string text)
        {
            if (!PostConfig.CorrectWakaba)
            {
                return text;
            }
            return ApplyReplaces(text, wakabaReplaces);
        }

        private string ApplySmileMarkup(string text)
        {
            if (!PostConfig.UseSmileMarkup)
            {
                return text;
            }
            var replaces = GetSmileReplaceDelegates2(
                    new Tuple<string, string, string>(PostConfig.SmileConfig.Bold, "[b]", "[/b]"),
                    new Tuple<string, string, string>(PostConfig.SmileConfig.Italic, "[i]", "[/i]"),
                    new Tuple<string, string, string>(PostConfig.SmileConfig.Monospace, "[code]", "[/code]"),
                    new Tuple<string, string, string>(PostConfig.SmileConfig.Over, "[o]", "[/o]"),
                    new Tuple<string, string, string>(PostConfig.SmileConfig.Spoiler, "[spoiler]", "[/spoiler]"),
                    new Tuple<string, string, string>(PostConfig.SmileConfig.Strike, "[s]", "[/s]"),
                    new Tuple<string, string, string>(PostConfig.SmileConfig.Sub, "[sub]", "[/sub]"),
                    new Tuple<string, string, string>(PostConfig.SmileConfig.Sup, "[sup]", "[/sup]"),
                    new Tuple<string, string, string>(PostConfig.SmileConfig.Under, "[u]", "[/u]")
                ).ToArray();
            return ApplyReplaces(text, replaces);
        }

        private IEnumerable<ReplaceDelegate> GetSmileReplaceDelegates2(params Tuple<string, string, string>[] data)
        {
            return data.SelectMany(d => GetSmileReplaceDelegates((d.Item1 ?? "").Replace(" ", ""), d.Item2, d.Item3));
        }

        private IEnumerable<string> SplitUtf3(string src)
        {
            for (int i = 0; i < src.Length; i++)
            {
                if (char.IsHighSurrogate(src[i]))
                {
                    var u = char.ConvertToUtf32(src, i);
                    yield return char.ConvertFromUtf32(u);
                }
            }
        }

        private IEnumerable<ReplaceDelegate> GetSmileReplaceDelegates(string symbols, string begin, string end)
        {
            return SplitUtf3(symbols).Select(c => ReplaceMarkup(c, begin, end));
        }

        private string CorrectLines(string text)
        {
            var lines = SplitString(text);
            return lines.Aggregate(new StringBuilder(), (sb, s) => (sb.Length > 0 ? sb.Append("\r\n") : sb).Append(s)).ToString();
        }

        private string ApplyReplaces(string source, ReplaceDelegate[] replaces)
        {
            var result = source;
            ReplaceDelegate lastReplace;
            do
            {
                lastReplace = replaces.FirstOrDefault(d => d(ref result));
            } while (lastReplace != null);
            return result;
        }

        // Пример:
        // aaaaaa^H^H^H
        // 000000000011
        // 012345678901
        // 123
        //    123
        //       123456
        //       ^

        private bool ReplaceStrike(ref string str)
        {
            var idx1 = str.IndexOf("^H", StringComparison.OrdinalIgnoreCase);
            if (idx1 >= 0)
            {
                var idx2 = idx1;
                var count = 1;
                while ((idx2 + 3) < str.Length && str[idx2 + 2] == '^' && str[idx2 + 3] == 'H')
                {
                    idx2 = idx2 + 2;
                    count++;
                }
                str = str.Substring(0, idx1 - count) + "[s]" + str.Substring(idx1 - count, count) + "[/s]" + str.Substring(idx2 + 2, str.Length - idx2 - 2);
                return true;
            }
            return false;
        }

        private ReplaceDelegate ReplaceAtLineStart(string mark, string begin, string end)
        {
            return (ref string str) =>
            {
                var splitted = SplitString(str).ToArray();
                var idx = FindIndex(splitted, s => s.StartsWith(mark, StringComparison.OrdinalIgnoreCase));
                if (idx >= 0)
                {
                    var line = splitted[idx];
                    var len = mark.Length;
                    splitted[idx] = begin + line.Substring(len, line.Length - len) + end;
                    str = splitted.Aggregate(new StringBuilder(), (sb, s) => sb.AppendLine(s)).ToString();
                    return true;
                }
                return false;
            };
        }

        private int FindIndex(string[] array, Func<string, bool> clause)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (clause(array[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        private IEnumerable<string> SplitString(string source)
        {
            using (var rd = new StringReader(source))
            {
                string line;
                do
                {
                    line = rd.ReadLine();
                    if (line != null)
                    {
                        yield return line;
                    }
                } while (line != null);
            }
        }

        private ReplaceDelegate ReplaceMarkup(string mark, string begin, string end)
        {
            return (ref string str) =>
            {
                var idx1 = str.IndexOf(mark, StringComparison.OrdinalIgnoreCase);
                if (idx1 >= 0)
                {
                    var idx2 = str.IndexOf(mark, idx1 + mark.Length, StringComparison.OrdinalIgnoreCase);
                    if (idx1 >= 0 && idx2 >= 0 && idx2 > idx1)
                    {
                        str = ReplaceAtIndexes(str, mark.Length, idx1, idx2, begin, end);
                    }
                    return true;
                }
                return false;
            };
        }

        // Пример
        // aaaa ** bcde ** dddddd
        // 0000000000111111111122
        // 0123456789012345678901
        // 12345--123456--1234567
        //      ^       ^
        //      1       2

        private string ReplaceAtIndexes(string source, int len, int idx1, int idx2, string atIdx1, string atIdx2)
        {
            return source.Substring(0, idx1) + atIdx1 + source.Substring(idx1 + len, idx2 - idx1 - len) + atIdx2 + source.Substring(idx2 + len, source.Length - idx2 - len);
        }         
    }
}