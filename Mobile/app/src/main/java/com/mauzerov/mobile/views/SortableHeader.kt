package com.mauzerov.mobile.views

class SortableHeader {
    private var buttons = mutableListOf<SortableButton>()

    operator fun plusAssign(button : SortableButton) {
        buttons.add(button);
    }

    fun update(main : SortableButton) {
        for (button in buttons) {
            if (main === button)
               continue

            button.state = SortableButton.ButtonState.None;
        }
    }
}