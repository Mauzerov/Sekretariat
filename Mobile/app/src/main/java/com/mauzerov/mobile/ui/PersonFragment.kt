package com.mauzerov.mobile.ui

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.view.get
import androidx.fragment.app.Fragment
import com.mauzerov.mobile.MainActivity
import com.mauzerov.mobile.R
import com.mauzerov.mobile.databinding.FragmentPersonBinding
import com.mauzerov.mobile.scripts.Where
import com.mauzerov.mobile.views.ResultTable
import com.mauzerov.mobile.scripts.filter

typealias TableRow = MutableMap<String, Comparable<String>>
typealias Table = MutableList<TableRow>

abstract class PersonFragment : Fragment() {

    private var _binding: FragmentPersonBinding? = null
    private lateinit var main: MainActivity
    private lateinit var tableName: String

    // This property is only valid between onCreateView and
    // onDestroyView.
    val binding get() = _binding!!
    private var table: Table? = null
    private var wheres: List<Where> = listOf()

    final override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentPersonBinding.inflate(inflater, container, false)
        beforeInit()
        val root: View = binding.root
        tableName = javaClass.simpleName.replace("Fragment", "");
        main = (requireActivity() as MainActivity)
        refresh()

        Log.d("FRAGMENT_ID", id.toString())
        binding.refreshView.setOnRefreshListener {
            refresh()
            binding.refreshView.isRefreshing = false
        }

        if (table == null || table!!.size == 0) {

        } else {
            Log.d("TABLE", tableName)
            loadResultTable(table!!);
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
    private fun refresh() {
        table = main.schoolData[tableName]!!

        if (main.wheres.containsKey(tableName))
            wheres = main.wheres[tableName]!!
        if (!(table == null || table!!.size == 0)) {
            table = main.schoolData[tableName]?.filter(wheres)
            loadResultTable(table!!)
        } else {
            (activity as MainActivity).makeErrorToast(R.string.toast_error_no_data)
        }
    }


    open fun beforeInit() {};
    open fun afterInit() {};

    final override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}