package com.mauzerov.mobile.views

import android.content.Context
import android.util.AttributeSet
import android.widget.GridLayout
import android.widget.GridView
import java.util.*

typealias TableRow = Dictionary<String, Comparable<Any>>
typealias Table = MutableList<Dictionary<String, Comparable<Any>>>

class ResultTable(context: Context?, attrs: AttributeSet?, defStyleAttr: Int, defStyleRes: Int) :
    GridLayout(context, attrs, defStyleAttr, defStyleRes) {

    constructor(context: Context?, attrs: AttributeSet?, defStyleAttr: Int) : this(context, attrs, defStyleAttr, 0)
    constructor(context: Context?, attrs: AttributeSet?) : this(context, attrs, 0, 0)
    constructor(context: Context?) : this(context, null, 0, 0)

    fun setup(data: Table) : Unit {

    }
}