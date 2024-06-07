public class RecursiveDescentParser
    {
        private string input;
        private int position;

        public string Parse(string expression)
        {
            input = expression;
            position = 0;
            return Doc();
        }

        private string Doc()
        {
            if (position >= input.Length)
            {
                return "";
            }

            string result = Element();
            result += Doc();

            return result;
        }

        private string Element()
        {
            if (position >= input.Length)
            {
                return "";
            }

            if (input[position] == '<')
            {
                if (MatchTag("<em>"))
                {
                    string result = "<em>";
                    result += Doc();
                    ExpectTag("</em>");
                    result += "</em>";
                    return result;
                }
                else if (MatchTag("<p>"))
                {
                    string result = "<p>";
                    result += Doc();
                    ExpectTag("</p>");
                    result += "</p>";
                    return result;
                }
                else if (MatchTag("<ol>"))
                {
                    string result = "<ol>";
                    result += List();
                    ExpectTag("</ol>");
                    result += "</ol>";
                    return result;
                }
                else if (MatchTag("<li>"))
                {
                    string result = "<li>";
                    result += Text();
                    ExpectTag("</li>");
                    result += "</li>";
                    return result;
                }
                else
                {
                    throw new Exception("Invalid tag at position " + position);
                }
            }
            else
            {
                return Text();
            }
        }

        private string List()
        {
            if (position >= input.Length || input.Substring(position, 5) == "</ol>")
            {
                return "";
            }

            string result = ListItem();
            result += List();

            return result;
        }

        private string ListItem()
        {
            if (MatchTag("<li>"))
            {
                string result = "<li>";
                result += Text();
                ExpectTag("</li>");
                result += "</li>";
                return result;
            }
            else
            {
                throw new Exception("Expected <li> at position " + position);
            }
        }

        private string Text()
        {
            if (position >= input.Length || input[position] == '<')
            {
                return "";
            }

            string result = Char();
            result += Text();

            return result;
        }

        private string Char()
        {
            if (position >= input.Length || input[position] == '<')
            {
                return "";
            }

            char currentChar = input[position];
            if (char.IsLetter(currentChar))
            {
                position++;
                return currentChar.ToString();
            }
            else
            {
                throw new Exception("Invalid character at position " + position);
            }
        }

        private bool MatchTag(string tag)
        {
            if (input.Substring(position).StartsWith(tag))
            {
                position += tag.Length;
                return true;
            }
            return false;
        }

        private void ExpectTag(string tag)
        {
            if (!MatchTag(tag))
            {
                throw new Exception("Expected " + tag + " at position " + position);
            }
        }
}