package com.mauzerov.mobile.views

import android.content.Context
import android.util.AttributeSet
import android.widget.GridLayout
import android.widget.TextView
import androidx.core.view.allViews
import com.mauzerov.mobile.scripts.Where
import com.mauzerov.mobile.scripts.addSpacesBeforeCapitalized
import com.mauzerov.mobile.scripts.sort
import com.mauzerov.mobile.scripts.filter

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

    fun setup(data: Table) : ResultTable {
        isFocusable = false
        database = data

        if (database?.size == 0)
            return this
        rowCount = 0
        this.removeAllViews()
        generateHeaders()
        generateFields()
        return this
    }

    private fun location(x : Int, y : Int) : LayoutParams {
        return LayoutParams(
            spec(x, CENTER),
            spec(y, CENTER)
        )
    }

    private fun generateHeaders()  {
        val keys = database?.get(0)?.keys?.toList()?.filter { x -> x != "UUID" }
        columns = keys?.size!!

        columnCount = columns
        rowCount++


        for (column in 0 until columns) {
            val button = SortableButton(context, header)
            button.text = keys[column].addSpacesBeforeCapitalized()
            button.columnTitle = keys[column]
            button.setOnClickListener {
                (it as SortableButton).state = it.state.next()
                it._header.update(it);

                when (it.state) {
                    SortableButton.ButtonState.Desc -> database?.sort(it.columnTitle, true)
                    SortableButton.ButtonState.Asc -> database?.sort(it.columnTitle)
                    SortableButton.ButtonState.None -> database?.sort("UUID")
                }
                generateFields()
            }
            addView(button, location(0, column))
        }
    }

    fun generateFields(wheres: List<Where> = listOf())  {
        val actions : MutableList<Runnable> = mutableListOf()

        for (view in allViews)
            if (view !is SortableButton)
                actions.add { removeView(view) }

        for (action in actions)
            action.run()

        val _database = database?.filter(wheres)

        for (row in 0 until database?.size!!) {
            rowCount++
            for (col in 0 until columns) {
                val tableRow = database?.get(row)!!
                val title = tableRow.keys.toList()[col]

                if (title == "UUID")
                    continue

                val value = tableRow[title].toString()

                val text = TextView(context)
                text.text = value.replace(" 00:00:00", "").replace(", ", ",\n")
                addView(text, location(row + 1, col))
            }
        }
    }
}