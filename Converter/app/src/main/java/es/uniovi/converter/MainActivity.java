package es.uniovi.converter;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;
import android.widget.EditText;
import android.widget.Toast;

public class MainActivity extends AppCompatActivity {

    double mEuroToDollar;
    EditText mEditTextEuros;
    EditText mEditTextDollars;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        mEuroToDollar = 1.34;

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
}
