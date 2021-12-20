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