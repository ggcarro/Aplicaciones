package es.uniovi.converter;

import androidx.appcompat.app.AppCompatActivity;

import android.content.res.AssetManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Environment;
import android.text.TextUtils;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;


public class MainActivity extends AppCompatActivity {

    double mConvertion;
    double mConvertionUSD;
    String[] mValues;
    EditText mEditTextUSD;
    EditText mEditTextCurrencie;
    Spinner array_spinnerInput;
    Spinner array_spinnerUSD;

    String UPDATE_URL = "http://apilayer.net/api/live?access_key=df9805d128149d5d4ff0944c52161147&";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        new UpdateRateTask().execute(UPDATE_URL);

        //Abrimos archivo currencie para obtener codigo ISO paises
        BufferedReader br = null;
        try {
            AssetManager am = getBaseContext().getAssets();
            InputStream is = am.open("currencies.csv");
            br = new BufferedReader(new InputStreamReader(is));

        } catch (FileNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }

        String line;

        List<String> list = new ArrayList<String>();

        //Leemos currencies.csv a lista formato bien
        try {
            while((line=br.readLine()) != null){
                String[] value = line.split(",");
                String[] notc = value[1].split("\"");
                list.add(notc[1]);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        //Añadimos contenido lista al spinner
        array_spinnerUSD = (Spinner)findViewById(R.id.USDSpinner);
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, android.R.layout.simple_spinner_item, list);
        array_spinnerUSD.setAdapter(adapter);
        array_spinnerInput = (Spinner)findViewById(R.id.InputSpinner);
        array_spinnerInput.setAdapter(adapter);


        //Vinculamos variables
        mEditTextUSD = (EditText) findViewById(R.id.editTextUSD);
        mEditTextCurrencie = (EditText) findViewById(R.id.editTextCurrencie);

    }

    public void onClickToUSD(View view){
        String spinnerText = array_spinnerInput.getSelectedItem().toString();
        mConvertion=0.0;
        for(int i=0; i<mValues.length; i++){
            String[] currencie = mValues[i].split(":");
            currencie[0]=currencie[0].substring(4);
            currencie[0]=currencie[0].substring(0,currencie[0].length()-1);
            if(currencie[0].equals(spinnerText)){
                mConvertion= Double.parseDouble(currencie[1]);
            }
        }

        String spinnerTextUSD = array_spinnerUSD.getSelectedItem().toString();
        mConvertionUSD=0.0;
        for(int i=0; i<mValues.length; i++){
            String[] currencie = mValues[i].split(":");
            currencie[0]=currencie[0].substring(4);
            currencie[0]=currencie[0].substring(0,currencie[0].length()-1);
            if(currencie[0].equals(spinnerTextUSD)){
                mConvertionUSD= Double.parseDouble(currencie[1]);
            }
        }

        if(mConvertion==0.0 || mConvertionUSD==0){
            Toast.makeText(getApplicationContext(), "Conversión no disponible",
                    Toast.LENGTH_SHORT).show();
        }

        convert(mEditTextUSD,mEditTextCurrencie,mConvertion, mConvertionUSD);
    }

    public void onClickToCurrencie(View view){
        String spinnerText = array_spinnerInput.getSelectedItem().toString();
        mConvertion=0.0;
        for(int i=0; i<mValues.length; i++){
            String[] currencie = mValues[i].split(":");
            currencie[0]=currencie[0].substring(4);
            currencie[0]=currencie[0].substring(0,currencie[0].length()-1);
            if(currencie[0].equals(spinnerText)){
                mConvertion= Double.parseDouble(currencie[1]);
            }

        }
        String spinnerTextUSD = array_spinnerUSD.getSelectedItem().toString();
        mConvertionUSD=0.0;
        for(int i=0; i<mValues.length; i++){
            String[] currencie = mValues[i].split(":");
            currencie[0]=currencie[0].substring(4);
            currencie[0]=currencie[0].substring(0,currencie[0].length()-1);
            if(currencie[0].equals(spinnerTextUSD)){
                mConvertionUSD= Double.parseDouble(currencie[1]);
            }
        }

        if(mConvertion==0.0 || mConvertionUSD==0){
            Toast.makeText(getApplicationContext(), "Conversión no disponible",
                    Toast.LENGTH_SHORT).show();
        }
        convert(mEditTextCurrencie,mEditTextUSD,1/mConvertion, 1/mConvertionUSD);
    }

    void convert(EditText editTextSource, EditText editTextDestination,
                 double ConversionFactor1, double ConversionFactor2) {

        String StringSource = editTextSource.getText().toString();

        double NumberSource;
        try {
            NumberSource = Double.parseDouble(StringSource);
        } catch (NumberFormatException nfe) {
            return;
        }
        double NumberDestination1 = NumberSource * ConversionFactor1;
        double NumberDestination = NumberDestination1 * ConversionFactor2;

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
                mConvertion = Double.parseDouble(result);
            else{
                mConvertion = -16.06;
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
            String rateString = rates.toString();
                //La cadena JSON viene con llaves que debemos eliminar
            rateString = rateString.substring(1);
            rateString=rateString.substring(0, rateString.length()-1);

            mValues = rateString.split(",");

            double usd = 1.0;
            return usd;
        }

        private double getCurrencyRateUsdRate(String url) throws IOException, JSONException {
            return parseDataFromNetwork(readStream(openUrl(url)));
        }
    }
}


