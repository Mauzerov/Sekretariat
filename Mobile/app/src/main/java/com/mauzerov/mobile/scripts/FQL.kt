package com.mauzerov.mobile.scripts

fun Table.filter(wheres: List<Where>) : Table {
    fun TableRow.matches(wheres: List<Where>) : Boolean {
        for(where in wheres) {
            val v = this[where.Key].toString()
            print(where.Value!!.toString().replace("%", ".*"))
            if (!when (where.Op) {
                    "=" ->  Regex(where.Value!!.toString().replace("%", ".*"), RegexOption.IGNORE_CASE).matches(v)
                    "<>" -> !Regex(where.Value!!.toString().replace("%", ".*"), RegexOption.IGNORE_CASE).matches(v)
                    "<=" -> v <= where.Value!!.toString()
                    "<" ->  v < where.Value!!.toString()
                    ">" ->  v > where.Value!!.toString()
                    ">=" -> v >= where.Value!!.toString()
                    else -> false // Wrong Operator
                }
            // Something Is Not Matching
            ) return false
        }
        // Everything Matches
        return true
    }

    return this.filter{ row -> row.matches(wheres) }.toMutableList()
}