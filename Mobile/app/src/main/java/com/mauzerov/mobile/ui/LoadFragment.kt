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
import com.mauzerov.mobile.scripts.XmlReader


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
            val location = binding.destination.text.toString()

            MainActivity.databaseLocation = location

            XmlReader.fill(location, database)
        }

        return root
    }
}