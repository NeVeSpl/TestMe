
export class StringUtils
{
    static Truncate40(text : string) : string
    {
        return text.length > 40 ? text.substring(0, 37) + "..." : text;
    }
}