package com.mauzerov.mobile.ui

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.fragment.app.DialogFragment
import com.mauzerov.mobile.MainActivity
import com.mauzerov.mobile.R
import com.mauzerov.mobile.databinding.HelpDialogBinding
import com.mauzerov.mobile.scripts.Where
import com.mauzerov.mobile.views.WhereStatement

class HelpDialog: DialogFragment() {

    private var _binding: HelpDialogBinding? = null
    // This property is only valid between onCreateView and
    // onDestroyView.
    val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = HelpDialogBinding.inflate(inflater, container, false)
        val rootView: View = binding.root

        binding.dismissHelpDialog.setOnClickListener {
            dismiss()
        }

        return rootView
    }

}