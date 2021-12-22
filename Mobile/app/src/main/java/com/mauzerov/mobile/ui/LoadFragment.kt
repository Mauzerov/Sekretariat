package com.mauzerov.mobile.ui

import android.os.Bundle
import android.text.Editable
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.mauzerov.mobile.MainActivity
import com.mauzerov.mobile.R
import com.mauzerov.mobile.databinding.FragmentLoadBinding
import com.mauzerov.mobile.scripts.CsvReader
import com.mauzerov.mobile.scripts.XmlReader
import java.io.File
import java.io.FilenameFilter


class LoadFragment : Fragment() {

    private var _binding: FragmentLoadBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentLoadBinding.inflate(inflater, container, false)
        val root: View = binding.root

        if (MainActivity.databaseLocation != null) {
            binding.destination.text = Editable.Factory.getInstance().newEditable(MainActivity.databaseLocation)
        }

        binding.loadButton.setOnClickListener {
            val database = (activity as MainActivity).schoolData
            if (binding.destination.text == null)
                return@setOnClickListener
            val location = binding.destination.text.toString()

            MainActivity.databaseLocation = location

            when(File(location).extension.lowercase()) {
                "xml" -> XmlReader.fill(location, database, {
                    (activity as MainActivity).makeErrorToast(R.string.toast_error_no_database_existence)
                },{
                    (activity as MainActivity).makeErrorToast(R.string.toast_message_database_existence)
                })

                "csv" -> CsvReader.fill(location, database, {
                    (activity as MainActivity).makeErrorToast(R.string.toast_error_no_database_existence)
                },{
                    (activity as MainActivity).makeErrorToast(R.string.toast_message_database_existence)
                })

                else -> (activity as MainActivity).makeErrorToast(R.string.toast_error_wrong_extension)
            }


        }

        return root
    }
}