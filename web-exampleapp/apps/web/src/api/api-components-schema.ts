const schema = {
    "components": [
        {
            "type": "table",
            "table": {
                "query": "AllEmployeesAdmin",
                "columns": [
                    {
                        "id": "Name",
                        "title": "Name",
                        "sortable": true,
                        "type": 1,
                        "filter": {
                            "variant": "single",
                            "field": "NameFilter",
                            "type": 1
                        }
                    }
                ]
            }
        },
        {
            "type": "table",
            "table": {
                "query": "AllProjectsAdmin",
                "columns": [
                    {
                        "id": "Name",
                        "title": "Name",
                        "sortable": true,
                        "type": 1,
                        "filter": {
                            "variant": "single",
                            "field": "NameFilter",
                            "type": 1
                        }
                    }
                ]
            }
        }
    ],
    "enumsMaps": {}
} as const;

export default schema;