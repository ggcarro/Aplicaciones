package es.uniovi.amigos;

import androidx.core.app.ActivityCompat;
import androidx.fragment.app.FragmentActivity;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.pm.PackageManager;
import android.location.Criteria;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.widget.EditText;

import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.Console;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;

public class MapsActivity extends FragmentActivity implements OnMapReadyCallback {

    String LIST_URL = "http://edico.serveo.net/api/amigo/";

    private GoogleMap mMap;
    String mUserID = null;
    String mUserName = null;
    double mLongi;
    double mLati;
    Amigo amigo = null;
    int UPDATE_PERIOD = 4000;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_maps);
        // Obtain the SupportMapFragment and get notified when the map is ready to be used.
        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map);
        mapFragment.getMapAsync(this);

        // Obtenemos Posición
        ActivityCompat.requestPermissions(this,new String[]{android.Manifest.permission.ACCESS_FINE_LOCATION}, 1);

        // Preguntamos usuario del que queremos conocer posicion
        askUserName();

        // Creamos temporizador para actualizar periodicamente posición
        Timer timer = new Timer();
        TimerTask updateAmigos = new UpdateAmigoPosition();
        timer.scheduleAtFixedRate(updateAmigos, 0, UPDATE_PERIOD);
    }

    // Obtenemos nombre de usuario
    public void askUserName() {
        AlertDialog.Builder alert = new AlertDialog.Builder(this);

        alert.setTitle("Settings");
        alert.setMessage("User name:");

        // Crear un EditText para obtener el nombre
        final EditText input = new EditText(this);
        alert.setView(input);

        alert.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int whichButton) {
                mUserName = input.getText().toString();
            }
        });

        alert.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
            public void onClick(DialogInterface dialog, int whichButton) {
                // Canceled.
            }
        });

        alert.show();
    }

    public void onRequestPermissionsResult(int requestCode, String permissions[], int[] grantResults) {
        switch (requestCode) {
            case 1: {
                if (grantResults.length > 0
                        && grantResults[0] == PackageManager.PERMISSION_GRANTED) {

                    // Llamar a una función para crear las peticiones de localización
                    SetupLocation();

                } else {

                    // Gestionar el caso de que no haya permisos
                }
                return;
            }
            // Se podrían gestionar más permisos
        }
    }
    void SetupLocation()
    {
        if (ActivityCompat.checkSelfPermission(this,
                android.Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED
                && ActivityCompat.checkSelfPermission(this,
                android.Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            return;
        }


        // Se debe adquirir una referencia al Location Manager del sistema
        LocationManager locationManager = (LocationManager) this.getSystemService(Context.LOCATION_SERVICE);

        // Se obtiene el mejor provider de posición
        Criteria criteria = new Criteria();
        String  provider = locationManager.getBestProvider(criteria, false);

        // Se crea un listener de la clase que se va a definir luego
        MyLocationListener locationListener = new MyLocationListener();

        // Se registra el listener con el Location Manager para recibir actualizaciones
        locationManager.requestLocationUpdates(provider, 0, 10, (LocationListener) locationListener);

        // Comprobar si se puede obtener la posición ahora mismo
        Location location = locationManager.getLastKnownLocation(provider);
        if (location != null) {
            // La posición actual es location
        } else {
            // Actualmente no se puede obtener la posición
        }
    }

    // Se define un Listener para escuchar por cambios en la posición
    class MyLocationListener implements LocationListener {
        @Override
        public void onLocationChanged(Location location) {
            // Se llama cuando hay una nueva posición para ese location provider
            mLati = location.getLatitude();
            mLongi = location.getLongitude();

        }

        // Se llama cuando cambia el estado
        @Override
        public void onStatusChanged(String provider, int status, Bundle extras) {}

        // Se llama cuando se activa el provider
        @Override
        public void onProviderEnabled(String provider) {}

        // Se llama cuando se desactiva el provider
        @Override
        public void onProviderDisabled(String provider) {}

    }


    /**
     * Manipulates the map once available.
     * This callback is triggered when the map is ready to be used.
     * This is where we can add markers or lines, add listeners or move the camera. In this case,
     * we just add a marker near Sydney, Australia.
     * If Google Play services is not installed on the device, the user will be prompted to install
     * it inside the SupportMapFragment. This method will only be triggered once the user has
     * installed Google Play services and returned to the app.
     */
    @Override
    public void onMapReady(GoogleMap googleMap) {
        mMap = googleMap;
    }


    public class Amigo {
        public String _name;
        public Double _longi;
        public Double _lati;


        public Amigo(String name, Double lati, Double longi) {
            _name = name;
            _lati = lati;
            _longi = longi;
        }
    }


    public class UpdateAmigoPosition extends TimerTask {
        public void run() {
            new ShowAmigosTask().execute(LIST_URL);
        }
        public class ShowAmigosTask extends AsyncTask<String, Void, List<Amigo>> {

            @Override
            protected void onPostExecute(List<Amigo> result) {
                drawAmigos(result);
            }
            @Override
            protected List<Amigo> doInBackground(String... urls) {
                try{
                    return getAmigos(urls[0]);
                } catch (IOException e) {
                    e.printStackTrace();
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                return null;
            }
            protected List<Amigo> getAmigos (String link) throws IOException, JSONException {
                InputStream stream = openUrl(link);
                String reader = readStream(stream);
                return parseDataFromNetwork(reader);
            }

            // Añadimos un Marcador por cada amigo al mapa
            public void drawAmigos(List<Amigo> listAmigos){
                mMap.clear();
                for (Amigo amigo: listAmigos) {
                    LatLng posicion = new LatLng(amigo._lati,amigo._longi);
                    mMap.addMarker(new MarkerOptions().position(posicion).title(amigo._name));
                }
            }


            protected InputStream openUrl(String urlString) throws IOException {
                URL url = new URL(urlString);
                HttpURLConnection conn = (HttpURLConnection) url.openConnection();
                conn.setReadTimeout(10000 /* milliseconds */);
                conn.setConnectTimeout(15000 /* milliseconds */);
                conn.setRequestMethod("GET");
                conn.setDoInput(true);

                // Starts the query
                conn.connect();
                return conn.getInputStream();
            }

            protected String readStream(InputStream urlStream) throws IOException {
                BufferedReader r = new BufferedReader(new InputStreamReader(urlStream));
                StringBuilder total = new StringBuilder();
                String line;
                while ((line = r.readLine()) != null) {
                    total.append(line);
                }
                return total.toString();
            }

            private List<Amigo> parseDataFromNetwork(String data)
                    throws IOException, JSONException {

                List<Amigo> amigosList = new ArrayList<Amigo>();

                JSONArray amigos = new JSONArray(data);

                for(int i = 0; i < amigos.length(); i++) {
                    JSONObject amigoObject = amigos.getJSONObject(i);

                    String name = amigoObject.getString("name");
                    String lati = amigoObject.getString("lati");
                    String longi = amigoObject.getString("longi");



                    double longiNumber;
                    double latiNumber;
                    try {
                        longiNumber = Double.parseDouble(longi);
                        latiNumber = Double.parseDouble(lati);
                    } catch (NumberFormatException nfe) {
                        continue;
                    }

                    if(mUserName != null) {
                        // Si el nombre que introducimos coincide con un usuario actualizamos su posición
                        if (mUserName.equals(name)) {
                            mUserID = amigoObject.getString("id");
                            longiNumber = mLongi;
                            latiNumber = mLati;
                            updatePosition();

                        }
                    }

                    amigosList.add(new Amigo(name, latiNumber, longiNumber));
                }

                return amigosList;
            }

            // Mandamos mensaje actualización posición
            protected void updatePosition () throws IOException, JSONException {

                URL url = new URL("http://edico.serveo.net/api/amigo/42");
                HttpURLConnection httpCon = (HttpURLConnection) url.openConnection();
                httpCon.setReadTimeout(10000 /* milliseconds */);
                httpCon.setConnectTimeout(15000 /* milliseconds */);

                httpCon.setDoOutput(true);
                httpCon.setDoInput(true);
                httpCon.setRequestProperty("Content-Type","application/JSON");

                httpCon.setRequestMethod("PUT");
                OutputStreamWriter out = new OutputStreamWriter(
                        httpCon.getOutputStream());

                String jObjSent="{\"ID\":\""+mUserID+"\",\"name\":\""+mUserName+"\",\"longi\":\""+mLati+"\",\"lati\":\""+mLongi+"\"}";
                out.write(jObjSent);
                out.close();

                httpCon.getInputStream();

            }
        }
    }


}
