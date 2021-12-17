package com.mauzerov.mobile.scripts

import java.util.*
typealias TableRow = Dictionary<String, Comparable<Any>>
typealias Table = MutableList<Dictionary<String, Comparable<Any>>>

class SchoolData
{
    var Students : Table = mutableListOf()
    var Teachers : Table = mutableListOf()
    var Employees : Table = mutableListOf()

    operator fun get(name: String) : Table {
            return when (name) {
                "Student" , "Students" -> Students
                "Teacher" , "Teachers" -> Teachers
                "Employee" , "Employees" -> Employees
                else -> throw NotImplementedError("Lack Of Tables Of Name: $name")
            }
    }

    operator fun set(name: String, value: Table) {
        when (name) {
            "Student" , "Students" -> Students = value
            "Teacher" , "Teachers" -> Teachers = value
            "Employee" , "Employees" -> Employees = value
            else -> throw NotImplementedError("Lack Of Tables Of Name: $name")
        }
    }
}
