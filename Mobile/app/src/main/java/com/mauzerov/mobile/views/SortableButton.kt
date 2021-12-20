package com.mauzerov.mobile.views

import android.annotation.SuppressLint
import android.content.Context
import android.util.AttributeSet
import android.util.Log
import android.view.MotionEvent
import android.view.View
import android.widget.Button
import androidx.appcompat.widget.AppCompatButton
import com.mauzerov.mobile.views.SortableButton.ButtonState



class SortableButton(context: Context, attrs: AttributeSet?, defStyleAttr: Int, header : SortableHeader) :
    androidx.appcompat.widget.AppCompatButton(context, attrs) {

    constructor(context: Context, attrs: AttributeSet?, header : SortableHeader) : this(context, attrs, 0, header)
    constructor(context: Context, header : SortableHeader) : this(context, null, 0, header)

    var _header : SortableHeader;

    enum class ButtonState {
        None, Asc, Desc {
            override fun next() : ButtonState { return None };
        };
        open fun next() : ButtonState{
            return values()[ordinal + 1]
        }
    }

    var state = ButtonState.None
        @SuppressLint("SetTextI18n")
        set(value) {
            field = value;
            if (value == ButtonState.None)
                this@SortableButton.text = columnTitle;
            else
                this@SortableButton.text = "$columnTitle $state"
        }
    var columnTitle = "UUID"

    init {
        header += this;
        _header = header;
    }
}