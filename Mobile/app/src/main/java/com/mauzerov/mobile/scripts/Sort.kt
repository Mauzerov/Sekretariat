package com.mauzerov.mobile.scripts

class Sort {
    companion object {
        fun dictionary(array: Table, field: String, reversed : Boolean = false) : Table {
            val n = array.size
            for (i in 1 until n)
            {
                val now = array[i]
                var j = i - 1

                while (j >= 0 && reversed xor (array[j][field]!! > now[field]!! as String))
                {
                    array[j + 1] = array[j]
                    j--
                }
                array[j + 1] = now
            }
            return array
        }
    }
}

fun Table.sort(field: String, reversed: Boolean = false) {
    Sort.dictionary(this, field, reversed);
}