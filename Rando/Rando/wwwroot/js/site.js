
 let polyline;

        function initialisationMap() {
            var steps = {
                "Départ": { "latitude": 42.87416, "longitude": 0.50079 },
                "Etape 1": { "latitude": 42.87364, "longitude": 0.48098 },
                "Etape 2": { "latitude": 42.86843, "longitude": 0.47186 },
                "Etape 3": { "latitude": 42.86159, "longitude": 0.46095 },
                "Arrivée": { "latitude": 42.87414, "longitude": 0.50082 },
            }

            var markersArray = [];

            let rambleMap = L.map('map').setView([42.87415, 0.500808], 13);
            L.tileLayer('https://{s}.tile.openstreetmap.fr/osmfr/{z}/{x}/{y}.png', {
                attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
                minZoom: 5,
                maxZoom: 25,
            }).addTo(rambleMap);

            for (step in steps) {
                var markers = L.marker([steps[step].latitude, steps[step].longitude]).addTo(rambleMap);
                markers.bindPopup("<p>" + step + "</p>");
                markersArray.push(markers);
            }
            var markersGroup = new L.featureGroup(markersArray);
            rambleMap.fitBounds(markersGroup.getBounds().pad(0.5));

            CreatePolyline("red");
            polyline.addTo(rambleMap);
        }
        window.onload = function () {
            initialisationMap();
        };
        var points = [];

        function CreatePolyline(colorChoice) {
            var points = [
                [42.87416, 0.50079],
                [42.87364, 0.48098],
                [42.86843, 0.47186],
                [42.86159, 0.46095],
                [42.87414, 0.50082]
            ];
            polyline = L.polyline(points, { color: colorChoice });
        }
