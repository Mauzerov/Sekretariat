package com.mauzerov.mobile

import android.os.Bundle
import android.util.Log
import android.view.Menu
import com.google.android.material.snackbar.Snackbar
import com.google.android.material.navigation.NavigationView
import androidx.navigation.findNavController
import androidx.navigation.ui.AppBarConfiguration
import androidx.navigation.ui.navigateUp
import androidx.navigation.ui.setupActionBarWithNavController
import androidx.navigation.ui.setupWithNavController
import androidx.drawerlayout.widget.DrawerLayout
import androidx.appcompat.app.AppCompatActivity
import com.mauzerov.mobile.databinding.ActivityMainBinding
import com.mauzerov.mobile.scripts.SchoolData
import com.mauzerov.mobile.scripts.XmlReader
import com.mauzerov.mobile.ui.SelectDialog
import kotlinx.coroutines.runBlocking
import java.util.*

typealias TableRow = MutableMap<String, Comparable<String>>
typealias Table = MutableList<TableRow>

class MainActivity : AppCompatActivity() {
    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding

    var schoolData = SchoolData()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.appBarMain.toolbar)

        val drawerLayout: DrawerLayout = binding.drawerLayout
        val navView: NavigationView = binding.navView
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        // Passing each menu ID as a set of Ids because each
        // menu should be considered as top level destinations.
        appBarConfiguration = AppBarConfiguration(
            setOf(
                R.id.nav_load, R.id.nav_student, R.id.nav_teacher, R.id.nav_employee
            ), drawerLayout
        )
        setupActionBarWithNavController(navController, appBarConfiguration)
        navView.setupWithNavController(navController)

        binding.appBarMain.fab.setOnClickListener {
            val id = navController.currentDestination?.id
            if (id == R.id.nav_load)
                return@setOnClickListener

            val dialog = SelectDialog(navController.currentDestination?.label.toString())
            dialog.show(supportFragmentManager, "selectDialog")
        }
        //XmlReader.fill("http://192.168.0.187/database.xml", schoolData)
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        // Inflate the menu; this adds items to the action bar if it is present.
        menuInflater.inflate(R.menu.main, menu)
        return true
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        return navController.navigateUp(appBarConfiguration) || super.onSupportNavigateUp()
    }

    companion object {
        var databaseLocation : String? = null;
    }
}