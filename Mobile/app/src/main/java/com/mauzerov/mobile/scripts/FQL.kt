package com.mauzerov.mobile.scripts

fun Table.filter(wheres: List<Where>) {
    fun TableRow.matches(wheres: List<Where>) : Boolean {
        for(where in wheres) {
            val v = this[where.Key].toString()
            if (!when (where.Op) {
                    "=" ->  Regex(where.Value!!.toString(), RegexOption.IGNORE_CASE).matches(v)
                    "<>" -> !Regex(where.Value!!.toString(), RegexOption.IGNORE_CASE).matches(v)
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

    this.filter{ row -> row.matches(wheres) }
}