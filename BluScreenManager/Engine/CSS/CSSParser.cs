using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BluEngine.Engine.CSS
{
    public class CSSParser
    {
        //css 2.1 macros (as per http://www.w3.org/TR/CSS2/syndata.html)
        public const String MACRO_UNICODE = "\\[0-9a-f]{1,6}(\r\n|[ \n\r\t\f])?";
        public const String MACRO_ESCAPE = MACRO_UNICODE + "|\\[^\n\r\f0-9a-f]";
        public const String MACRO_NONASCII = "[^\\0-\\237]";
        public const String MACRO_NMCHAR = "[_a-z0-9-]|" + MACRO_NONASCII + "|" + MACRO_ESCAPE;
        public const String MACRO_NAME = MACRO_NMCHAR+"+";
        public const String MACRO_HASH = MACRO_NMCHAR + "+";
        public const String MACRO_NMSTART = "[_a-z]|" + MACRO_NONASCII + "|" + MACRO_ESCAPE;
        public const String MACRO_IDENT = "[-]?"+MACRO_NMSTART+MACRO_NMCHAR+"*";
        public const String MACRO_NUM = "[0-9]+|[0-9]*\\.[0-9]+";
        public const String MACRO_STRING1 = "\"([^\n\r\f\\\"]|\\"+MACRO_NL+"|"+MACRO_ESCAPE+")*\"";
        public const String MACRO_STRING2 = "'([^\n\r\f\\']|\\"+MACRO_NL+"|"+MACRO_ESCAPE+")*'";
        public const String MACRO_STRING = MACRO_STRING1+"|"+MACRO_STRING2;
        public const String MACRO_BADSTRING1 = "\"([^\n\r\f\\\"]|\\"+MACRO_NL+"|"+MACRO_ESCAPE+")*\\?";
        public const String MACRO_BADSTRING2 = "\'([^\n\r\f\\']|\\"+MACRO_NL+"|"+MACRO_ESCAPE+")*\\?";
        public const String MACRO_BADSTRING = MACRO_BADSTRING1+"|"+MACRO_BADSTRING2;
        public const String MACRO_BADCOMMENT1 = "\\/\\*[^*]*\\*+([^/*][^*]*\\*+)*";
        public const String MACRO_BADCOMMENT2 = "\\/\\*[^*]*(\\*+[^/*][^*]*)*";
        public const String MACRO_BADCOMMENT = MACRO_BADCOMMENT1+"|"+MACRO_BADCOMMENT2;
        public const String MACRO_BADURI1 = "url\\("+MACRO_W+"([!#$%&*-~]|"+MACRO_NONASCII+"|"+MACRO_ESCAPE+")*"+MACRO_W;
        public const String MACRO_BADURI2 = "url\\("+MACRO_W+MACRO_STRING+MACRO_W;
        public const String MACRO_BADURI3 = "url\\("+MACRO_W+MACRO_BADSTRING;
        public const String MACRO_BADURI = MACRO_BADURI1+"|"+MACRO_BADURI2+"|"+MACRO_BADURI3;
        public const String MACRO_NL = "\n|\r\n|\r|\f";
        public const String MACRO_W = "[ \t\r\n\f]*";
        public static String[] MACROS
        {
            get
            {
                if (macros == null)
                {
                    macros = new String[]
                    {
                        MACRO_UNICODE, MACRO_ESCAPE, MACRO_NONASCII, MACRO_NMCHAR, 
                        MACRO_NAME, MACRO_HASH, MACRO_NMSTART, MACRO_IDENT, 
                        MACRO_NUM, MACRO_STRING1, MACRO_STRING2, MACRO_STRING, 
                        MACRO_BADSTRING1, MACRO_BADSTRING2, MACRO_BADSTRING, MACRO_BADCOMMENT1, 
                        MACRO_BADCOMMENT2, MACRO_BADCOMMENT, MACRO_BADURI1, MACRO_BADURI2, 
                        MACRO_BADURI3, MACRO_BADURI, MACRO_NL, MACRO_W
                    };
                }
                return macros;
            }
        }
        private static String[] macros = null;

        //css 2.1 tokens (as per http://www.w3.org/TR/CSS2/syndata.html)
        public const String TOKEN_IDENT = MACRO_IDENT;
        public const String TOKEN_ATKEYWORD = "@" + MACRO_NAME;
        public const String TOKEN_STRING = MACRO_STRING;
        public const String TOKEN_BAD_STRING = MACRO_BADSTRING;
        public const String TOKEN_BAD_URI = MACRO_BADURI;
        public const String TOKEN_BAD_COMMENT = MACRO_BADCOMMENT;
        public const String TOKEN_HASH = "#" + MACRO_NAME;
        public const String TOKEN_NUMBER = MACRO_NUM;
        public const String TOKEN_PERCENTAGE = MACRO_NUM + "%";
        public const String TOKEN_DIMENSION = MACRO_NUM + MACRO_IDENT;
        public const String TOKEN_URI = "url\\(" + MACRO_W + MACRO_STRING + MACRO_W + "\\)|url\\(" + MACRO_W + "([!#$%&*-\\[\\]-~]|" + MACRO_NONASCII + "|" + MACRO_ESCAPE + ")*" + MACRO_W + "\\)";
        public const String TOKEN_UNICODE_RANGE = "u\\+[0-9a-f?]{1,6}(-[0-9a-f]{1,6})?";
        public const String TOKEN_CDO = "<!--";
        public const String TOKEN_CDC = "-->";
        public const String TOKEN_COLON = ":";
        public const String TOKEN_SEMICOLON = ";";
        public const String TOKEN_BRACKET_LEFT = "\\{";
        public const String TOKEN_BRACKET_RIGHT = "\\}";
        public const String TOKEN_PARENTHESIS_LEFT = "\\(";
        public const String TOKEN_PARENTHESIS_RIGHT = "\\)";
        public const String TOKEN_SQ_BRACKET_LEFT = "\\[";
        public const String TOKEN_SQ_BRACKET_RIGHT = "\\]";
        public const String TOKEN_S = "[ \t\r\n\f]+";
        public const String TOKEN_COMMENT = "\\/\\*[^*]*\\*+([^/*][^*]*\\*+)*\\/";
        public const String TOKEN_FUNCTION = MACRO_IDENT+"\\(";
        public const String TOKEN_INCLUDES = "~=";
        public const String TOKEN_DASHMATCH = "|=";
        //public const String TOKEN_DELIM = "any other character not matched by the above rules, and neither a single nor a double quote...";
        public static String[] TOKENS
        {
            get
            {
                if (tokens == null)
                {
                    tokens = new String[]
                    {
                         TOKEN_IDENT, TOKEN_ATKEYWORD, TOKEN_STRING, TOKEN_BAD_STRING,
                         TOKEN_BAD_URI, TOKEN_BAD_COMMENT, TOKEN_HASH, TOKEN_NUMBER,
                         TOKEN_PERCENTAGE, TOKEN_DIMENSION, TOKEN_URI, TOKEN_UNICODE_RANGE,
                         TOKEN_CDO, TOKEN_CDC, TOKEN_COLON, TOKEN_SEMICOLON,
                         TOKEN_BRACKET_LEFT, TOKEN_BRACKET_RIGHT, TOKEN_PARENTHESIS_LEFT, TOKEN_PARENTHESIS_RIGHT,
                         TOKEN_SQ_BRACKET_LEFT, TOKEN_SQ_BRACKET_RIGHT, TOKEN_S, TOKEN_COMMENT,
                         TOKEN_FUNCTION, TOKEN_INCLUDES, TOKEN_DASHMATCH
                    };
                }
                return tokens;
            }
        }
        private static String[] tokens = null;
    }
}
