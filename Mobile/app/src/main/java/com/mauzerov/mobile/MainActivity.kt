package com.mauzerov.mobile

import android.content.pm.ActivityInfo
import android.os.Bundle
import android.util.Log
import android.view.Menu
import android.view.MenuItem
import android.widget.Toast
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
import com.mauzerov.mobile.scripts.Where
import com.mauzerov.mobile.scripts.XmlReader
import com.mauzerov.mobile.ui.HelpDialog
import com.mauzerov.mobile.ui.SelectDialog
import kotlinx.coroutines.runBlocking
import java.util.*

typealias TableRow = MutableMap<String, Comparable<String>>
typealias Table = MutableList<TableRow>

class MainActivity : AppCompatActivity() {
    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding

    var schoolData = SchoolData()
    var wheres: MutableMap<String, List<Where>> = mutableMapOf()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        requestedOrientation = ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE
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
            val tableName = navController.currentDestination?.label.toString()
            if (id == R.id.nav_load) {
                makeErrorToast(R.string.toast_error_no_table)
                return@setOnClickListener
            }

            if (schoolData[tableName]!!.size == 0){
                makeErrorToast(R.string.toast_error_no_data)
                return@setOnClickListener
            }

            val dialog = SelectDialog(tableName)
            dialog.show(supportFragmentManager, "selectDialog")
            wheres[tableName] = dialog.wheres
        }
    }

    override fun onCreateOptionsMenu(menu: Menu): Boolean {
        // Inflate the menu; this adds items to the action bar if it is present.
        menuInflater.inflate(R.menu.main, menu)
        return true
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when(item.itemId){
            R.id.action_help -> {
                HelpDialog().show(supportFragmentManager, "helpDialog")
                true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }

    override fun onSupportNavigateUp(): Boolean {
        val navController = findNavController(R.id.nav_host_fragment_content_main)
        return navController.navigateUp(appBarConfiguration) || super.onSupportNavigateUp()
    }

    fun makeErrorToast(id: Int) {
        runOnUiThread { Toast.makeText(this, id, Toast.LENGTH_SHORT).show() }
    }

    companion object {
        var databaseLocation : String? = null
    }
}