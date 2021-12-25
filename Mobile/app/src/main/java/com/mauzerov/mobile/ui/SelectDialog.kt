package com.mauzerov.mobile.ui

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.fragment.app.DialogFragment
import com.mauzerov.mobile.MainActivity
import com.mauzerov.mobile.R
import com.mauzerov.mobile.databinding.SelectDialogBinding
import com.mauzerov.mobile.scripts.Where
import com.mauzerov.mobile.views.WhereStatement

class SelectDialog(private val tableName: String) : DialogFragment() {

    private var _binding: SelectDialogBinding? = null
    private lateinit var group: RadioGroup
    var wheres = mutableListOf<Where>()
    // This property is only valid between onCreateView and
    // onDestroyView.
    val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = SelectDialogBinding.inflate(inflater, container, false)
        val rootView: View = binding.root
        val table = (activity as MainActivity).schoolData[tableName]!!
        group = RadioGroup(context)

        for (key in table[0].keys.filter { e -> e != "UUID" }) {
            val box = RadioButton(context)
            box.text = key
            group.addView(box)
        }
        binding.fieldsLayout.addView(group)

        binding.closeButton.setOnClickListener {
            dismiss()
        }

        binding.addButton.setOnClickListener { it as ImageButton
            if (group.checkedRadioButtonId == -1) {
                (activity as MainActivity).makeErrorToast(R.string.toast_error_no_field_selected)
                return@setOnClickListener
            }

            val where = Where()
            where.Key = binding.root.findViewById<RadioButton>(group.checkedRadioButtonId).text.toString()
            where.Value = binding.whereFieldInsert.text.toString()

            val op = binding.operandInsert.text.toString()
            if (!Where.Operands.contains(op)) {
                (activity as MainActivity).makeErrorToast(R.string.toast_error_no_operand)
                return@setOnClickListener
            }
            where.Op = op
            wheres.add(where)
            WhereStatement(context, this, binding.whereStatements, where)
        }

        return rootView
    }

}