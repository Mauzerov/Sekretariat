package com.mauzerov.mobile.scripts

import java.lang.Exception
import java.net.HttpURLConnection
import java.net.URL
import java.util.*
import javax.xml.parsers.DocumentBuilderFactory

class XmlReader {
    companion object {
        fun fill(_source: String, destination: SchoolData, onError: () -> Unit = {}, onSuccess: () -> Unit = {}) {
            if (_source == "")
                return

            for (table in destination.tables())
                destination[table]!!.clear()

            try {
                val source = if (!Regex("https?://.*").matches(_source)) "http://$_source" else _source
                Thread {
                    try {
                        val url = URL(source)

                        val connection = url.openConnection() as HttpURLConnection
                        connection.connectTimeout = 4 * 1000

                        val builder = DocumentBuilderFactory.newInstance().newDocumentBuilder()

                        val document = builder.parse(connection.inputStream)

                        val root = document.documentElement

                        for (i in 0 until root.childNodes.length) {
                            val child = root.childNodes.item(i)
                            if (!child.hasAttributes())
                                continue
                            val row = mutableMapOf<String, Comparable<String>>()
                            for (ai in 0 until child.attributes.length) {
                                val attr = child.attributes.item(ai)
                                row[attr.nodeName] = attr.nodeValue
                            }
                            row["UUID"] = UUID.randomUUID().toString()
                            destination[child.nodeName]?.add(row)
                        }
                        onSuccess()
                    } catch (e: Exception) {
                        onError()
                        android.util.Log.e("Exception", e.message?:"")
                    }
                }.start()
            } catch (e: Exception) {
                onError()
                android.util.Log.e("Exception", e.message?:"")
            }
        }
    }
}

class CsvReader {
    companion object {
        fun fill(_source: String, destination: SchoolData, onError: () -> Unit = {}, onSuccess: () -> Unit = {}) {

        }
    }
}