package com.mauzerov.mobile.scripts

fun Table.filter(wheres: List<Where>) : Table {
    fun TableRow.matches(wheres: List<Where>) : Boolean {
        for(where in wheres) {
            val v = this[where.Key].toString()
            if (!when (where.Op) {
                    "=" ->  Regex(where.Value!!.toString().replace("%", ".*"), RegexOption.IGNORE_CASE).matches(v)
                    "<>" -> !Regex(where.Value!!.toString().replace("%", ".*"), RegexOption.IGNORE_CASE).matches(v)
                    "<=" -> v <= where.Value!!.toString()
                    "<" ->  v < where.Value!!.toString()
                    ">" ->  v > where.Value!!.toString()
                    ">=" -> v >= where.Value!!.toString()
                    else -> false
                }
            ) return false
        }
        return true
    }

    return this.filter{ row -> row.matches(wheres) }.toMutableList()
}