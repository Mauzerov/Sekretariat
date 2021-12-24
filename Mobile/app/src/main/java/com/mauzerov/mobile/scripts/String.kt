package com.mauzerov.mobile.scripts

fun String.addSpacesBeforeCapitalized() : String
{
    if (this.isBlank())
        return ""
    val newText = StringBuilder(this.length * 2)
    newText.append(this[0])
    for (i in 1 until this.length)
    {
        if (this[i].isUpperCase() && this[i - 1] != ' ')
            newText.append(' ')
        newText.append(this[i])
    }
    return newText.toString()
}

fun String.splitNotInCommas(delimiter: Char) : List<String>
{
    return this.splitNotInCommas(delimiter.toString())
}
fun String.splitNotInCommas(delimiter: String) : List<String>
{
    val ret = mutableListOf<String>()
    var lastIndex = 0
    var index = this.indexOf(delimiter)
    while (index >= 0)
    {
        val subS = this.substring(lastIndex, index);
        val count = subS.count{f -> f == '\"'};
        if ((count and 1) == 0)
        {
            ret.add(subS);
            lastIndex = index + delimiter.length;
        }
        index = this.indexOf(delimiter, index + delimiter.length);
    }
    ret.add(this.substring(lastIndex, this.length));
    return ret;
}