package es.uniovi.converter;

import androidx.appcompat.app.AppCompatActivity;

import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;


public class MainActivity extends AppCompatActivity {

    double mEuroToDollar;
    EditText mEditTextEuros;
    EditText mEditTextDollars;

    String UPDATE_URL = "http://apilayer.net/api/live?access_key=df9805d128149d5d4ff0944c52161147&";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        new UpdateRateTask().execute(UPDATE_URL);

        //mEuroToDollar = 1.34;

        mEditTextEuros = (EditText) findViewById(R.id.editTextEuros);
        mEditTextDollars = (EditText) findViewById(R.id.editTextDollars);


    }

    public void onClickToEuros(View view){

        convert(mEditTextEuros,mEditTextDollars,mEuroToDollar);
    }

    public void onClickToDollars(View view){
        convert(mEditTextDollars,mEditTextEuros,1/mEuroToDollar);
    }

    void convert(EditText editTextSource, EditText editTextDestination,
                 double ConversionFactor) {

        String StringSource = editTextSource.getText().toString();

        double NumberSource;
        try {
            NumberSource = Double.parseDouble(StringSource);
        } catch (NumberFormatException nfe) {
            return;
        }
        double NumberDestination = NumberSource * ConversionFactor;

        String StringDestination = Double.toString(NumberDestination);

        editTextDestination.setText(StringDestination);
    }

    public class UpdateRateTask extends
            AsyncTask<String, Void, String> {



        @Override
        protected String doInBackground(String... urls) {
            try {
                return String.valueOf(getCurrencyRateUsdRate(urls[0]));
            } catch (IOException | JSONException e) {
                e.printStackTrace();
            }
            return null;
        }

        @Override
        protected void onPostExecute(String result) {
            if(result != null)
                mEuroToDollar = Double.parseDouble(result);
            else{
                mEuroToDollar = -16.06;
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

        private double parseDataFromNetwork(String data) throws JSONException {
            JSONObject object = new JSONObject(data);
            JSONObject rates = object.getJSONObject("quotes");
            double usd = rates.getDouble("USDEUR");


            return usd;
        }

        private double getCurrencyRateUsdRate(String url) throws IOException, JSONException {
            return parseDataFromNetwork(readStream(openUrl(url)));
        }
    }
}


