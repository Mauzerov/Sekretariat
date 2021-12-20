package com.mauzerov.mobile.views

import android.content.Context
import android.util.AttributeSet
import android.view.View
import android.widget.DatePicker
import android.widget.GridLayout
import android.widget.GridView
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.core.view.children
import com.mauzerov.mobile.R
import com.mauzerov.mobile.scripts.addSpacesBeforeCapitalized
import java.text.SimpleDateFormat
import java.util.*

typealias TableRow = MutableMap<String, Comparable<String>>
typealias Table = MutableList<TableRow>

class ResultTable(context: Context?, attrs: AttributeSet?, defStyleAttr: Int, defStyleRes: Int) :
    GridLayout(context, attrs, defStyleAttr, defStyleRes) {

    constructor(context: Context?, attrs: AttributeSet?, defStyleAttr: Int) : this(context, attrs, defStyleAttr, 0)
    constructor(context: Context?, attrs: AttributeSet?) : this(context, attrs, 0, 0)
    constructor(context: Context?) : this(context, null, 0, 0)

    private var database: Table? = null
    private var header: SortableHeader = SortableHeader()
    private var columns: Int = 0

    fun setup(data: Table) {
        database = data

        if (database?.size == 0)
            return
        rowCount = 0
        this.removeAllViews()
        generateHeaders()
        generateFields()
    }

    private fun location(x : Int, y : Int) : LayoutParams {
        return LayoutParams(
            spec(x, CENTER),
            spec(y, CENTER)
        )
    }

    private fun generateHeaders()  {
        columns = database?.get(0)?.keys?.toList()?.size!!
        val names = database?.get(0)?.keys!!

        columnCount = columns
        rowCount++


        for (column in 0 until columns) {
            val button = SortableButton(context, header)
            button.text = names.toList()[column].addSpacesBeforeCapitalized()
            button.columnTitle = button.text.toString()
            button.setOnClickListener {
                (it as SortableButton).state = it.state.next()
                it._header.update(it);
            }
            addView(button, location(0, column))
        }
    }

    private fun generateFields()  {
        for (row in 0 until database?.size!!) {
            rowCount++
            for (col in 0 until columns) {
                val tableRow = database?.get(row)!!
                val value = tableRow[tableRow.keys.toList()[col]].toString();

                val text = TextView(context)
                text.text = value.replace(" 00:00:00", "").replace(", ", ",\n")
                addView(text, location(row + 1, col))
            }
        }
    }
}