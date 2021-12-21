package com.mauzerov.mobile.scripts

class Where {
    var Key: String? = null
    var Value: Comparable<String>? = null;
    var Op: String? = null

    companion object {
        var Operands = mutableListOf("=", "<>", "<", "<=", ">=", ">")
        fun Human(where: Where): String {
            return where.Human()
        }
    }

    fun Human(): String {
        return "{$Key} {$Op} \"{$Value}\"";
    }

}