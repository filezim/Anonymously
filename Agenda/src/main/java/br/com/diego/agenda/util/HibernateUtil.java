package br.com.diego.agenda.util;

import org.hibernate.SessionFactory;
import org.hibernate.boot.registry.StandardServiceRegistryBuilder;
import org.hibernate.cfg.Configuration;
import org.hibernate.service.ServiceRegistry;

public class HibernateUtil {

	private static SessionFactory buildSessionFactory = sessionFactory();

	public static SessionFactory getBuildSessionFactory() {
		return buildSessionFactory;
	}

	private static SessionFactory sessionFactory() {
		try {
			Configuration configuration = new Configuration().configure();
			ServiceRegistry registry = new StandardServiceRegistryBuilder().applySettings(configuration.getProperties())
					.build();

			SessionFactory factory = configuration.buildSessionFactory(registry);

			return factory;

		} catch (Throwable ex) {
			System.err.println("Não foi possível criar a sessão" + ex);
			throw new ExceptionInInitializerError(ex);
		}
	}
}