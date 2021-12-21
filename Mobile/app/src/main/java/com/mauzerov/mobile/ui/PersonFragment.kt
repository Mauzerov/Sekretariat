package com.mauzerov.mobile.ui

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.view.get
import androidx.fragment.app.Fragment
import com.mauzerov.mobile.MainActivity
import com.mauzerov.mobile.databinding.FragmentPersonBinding
import com.mauzerov.mobile.views.ResultTable

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
        val tableName = javaClass.simpleName.replace("Fragment", "");
        val table = (requireActivity() as MainActivity).schoolData[tableName];

        Log.d("FRAGMENT_ID", id.toString())
        binding.refreshView.setOnRefreshListener {
            if (!(table == null || table.size == 0))
                loadResultTable(table)

            binding.refreshView.isRefreshing = false
        }

        if (table == null || table.size == 0) {
//        if (table.size == 0) {
//            val text = TextView(requireContext());
//            text.text = requireContext().getText(R.string.data_missing_error)
//            binding.root.addView(text)
//        }
        } else {
            Log.d("TABLE", tableName)
            loadResultTable(table);
        }

        afterInit()
        return root
    }

    private fun loadResultTable(table: Table) {
        val r = ResultTable(context).setup(table)
        val holder = binding.resultTableHolder
        holder.removeAllViews()
        holder.addView(r)
        r.generateFields()
    }


    open fun beforeInit() {};
    open fun afterInit() {};

    final override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}