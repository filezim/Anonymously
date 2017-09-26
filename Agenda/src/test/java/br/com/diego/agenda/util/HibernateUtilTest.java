package br.com.diego.agenda.util;

import org.hibernate.Session;
import org.junit.Test;

public class HibernateUtilTest {
	@Test
	public void connect() {
		Session session = HibernateUtil.getBuildSessionFactory().openSession();
		session.close();
		HibernateUtil.getBuildSessionFactory().close();
	}

}