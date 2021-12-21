package com.mauzerov.mobile.ui

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.DialogFragment
import com.mauzerov.mobile.R

class SelectDialog(private val tableName: String) : DialogFragment() {

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val rootView: View = inflater.inflate(R.layout.select_dialog, container, false)


        return super.onCreateView(inflater, container, savedInstanceState)
    }
}