package br.com.diego.agenda.util;

import javax.persistence.EntityManagerFactory;
import javax.persistence.Persistence;

public class App {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		EntityManagerFactory fac = Persistence.createEntityManagerFactory("genda");
				

	}

}
