package com.mauzerov.mobile.scripts

typealias TableRow = MutableMap<String, Comparable<String>>
typealias Table = MutableList<TableRow>

class SchoolData
{
    var Students : Table = mutableListOf()
    var Teachers : Table = mutableListOf()
    var Employees : Table = mutableListOf()

    operator fun get(name: String) : Table? {
            return when (name) {
                "Student" , "Students" -> Students
                "Teacher" , "Teachers" -> Teachers
                "Employee" , "Employees" -> Employees
                else -> null
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

    fun tables() : List<String> {
        return this::class.java.declaredFields.iterator().asSequence().map { f -> f.name }.toList()
    }
}
