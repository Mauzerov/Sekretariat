package com.mauzerov.mobile.ui

import android.content.res.Resources
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import androidx.fragment.app.Fragment
import com.mauzerov.mobile.MainActivity
import com.mauzerov.mobile.R
import com.mauzerov.mobile.databinding.FragmentPersonBinding
import com.mauzerov.mobile.views.ResultTable
import java.util.*

typealias TableRow = MutableMap<String, Comparable<String>>
typealias Table = MutableList<TableRow>

abstract class PersonFragment : Fragment() {

    private var _binding: FragmentPersonBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    val binding get() = _binding!!

    final override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentPersonBinding.inflate(inflater, container, false)
        beforeInit()
        val root: View = binding.root
        val tabelName = javaClass.simpleName.replace("Fragment", "");
        val table = (requireActivity() as MainActivity).schoolData[tabelName];
//        if (table.size == 0) {
//            val text = TextView(requireContext());
//            text.text = requireContext().getText(R.string.data_missing_error)
//            binding.root.addView(text)
//        }
        Log.d("TABLE", tabelName)
        binding.button.setOnClickListener {
            val r = ResultTable(context)
            r.setup((activity as MainActivity).schoolData[tabelName])
            r.isFocusable = false
            val holder = binding.resultTableHolder
            //r.layoutParams = ViewGroup.LayoutParams(Resources.getSystem().displayMetrics.widthPixels, Resources.getSystem().displayMetrics.heightPixels)
            holder.removeAllViews()
            holder.addView(r)//, r.layoutParams)
            //binding.resultTableHolder.addView(r)
        }

        afterInit()
        return root
    }

    open fun beforeInit() {};
    open fun afterInit() {};

    final override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}