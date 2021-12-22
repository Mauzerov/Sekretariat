package com.mauzerov.mobile.views

import android.content.Context
import android.view.ViewGroup
import android.widget.ImageButton
import android.widget.LinearLayout
import android.widget.TextView
import com.mauzerov.mobile.scripts.Where
import com.mauzerov.mobile.ui.SelectDialog

class WhereStatement(private val context: Context?,
                     private val parent: SelectDialog,
                     private val root: ViewGroup,
                     where: Where)
{
    lateinit var layout: LinearLayout

    init {
        val label = TextView(context)
        label . text = where.Human()
        label . textSize = 20f

        val button = ImageButton(context)
        button.setImageResource(android.R.drawable.ic_delete)

        val buttonParams = LinearLayout.LayoutParams(60, 60)

        button.setOnClickListener {
            parent.wheres.remove(where)
            root.removeView(layout)
        }

        layout = LinearLayout(context)
        layout.orientation = LinearLayout.HORIZONTAL
        layout.addView(button, buttonParams)
        layout.addView(label)

        root.addView(layout)
    }
}