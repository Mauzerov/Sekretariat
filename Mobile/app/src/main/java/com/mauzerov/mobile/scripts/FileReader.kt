package com.mauzerov.mobile.scripts

import java.net.HttpURLConnection
import java.net.MalformedURLException
import java.net.SocketTimeoutException
import java.net.URL
import java.util.*
import javax.xml.parsers.DocumentBuilderFactory

class XmlReader {
    companion object {
        fun read(source: String) : Table {
            val ret = mutableListOf<TableRow>()

            val url = URL(source)
            return ret
        }

        fun fill(_source: String, destination: SchoolData) {
            val source = if (!Regex("https?://.*").matches(_source)) "http://$_source" else _source
            try {
                Thread {
                    val url = URL(source)

                    val connection = url.openConnection() as HttpURLConnection
                    connection.connectTimeout = 60 * 1000

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
                }.start()
            } catch (e: SocketTimeoutException) {
                android.util.Log.e("SocketTimeoutException", e.message?:"")
            } catch (e: MalformedURLException) {
                android.util.Log.e("MalformedURLException", e.message?:"")
            }
        }
    }
}

class Csv {
    companion object {
        fun read(source: String) : Table {
            val ret = mutableListOf<TableRow>()

            val url = URL(source)
            return ret
        }
    }
}