package com.mauzerov.mobile.scripts

import java.net.HttpURLConnection
import java.net.SocketTimeoutException
import java.net.URL
import javax.xml.parsers.DocumentBuilderFactory

class XmlReader {
    companion object {
        fun read(source: String) : Table {
            val ret = mutableListOf<TableRow>()

            val url = URL(source)
            return ret;
        }

        fun fill(source: String, destination: SchoolData) {
            try {
                Thread {
                    val url = URL(source)

                    val file = java.io.File(source)

                    val connection = url.openConnection() as HttpURLConnection
                    connection.connectTimeout = 60 * 1000

                    val builder = DocumentBuilderFactory.newInstance().newDocumentBuilder()

                    val document = builder.parse(connection.inputStream);

                    val root = document.documentElement;

                    for (i in 0 until root.childNodes.length) {
                        val child = root.childNodes.item(i)
                        if (!child.hasAttributes())
                            continue
                        val row = mutableMapOf<String, Comparable<String>>()
                        for (ai in 0 until child.attributes.length) {
                            val attr = child.attributes.item(ai)
                            //print("${attr.nodeName}, ${attr.nodeValue} ")
                            row[attr.nodeName] = attr.nodeValue
                        }
                        destination[child.nodeName].add(row)
                    }
                }.start()
            } catch (e: SocketTimeoutException) {
                android.util.Log.e("SocketTimeoutException", e.message?:"")
            }

        }
    }
}

class Csv {
    companion object {
        fun read(source: String) : Table {
            val ret = mutableListOf<TableRow>()

            val url = URL(source)
            return ret;
        }
    }
}