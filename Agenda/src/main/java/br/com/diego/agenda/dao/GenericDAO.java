package br.com.diego.agenda.dao;

import java.lang.reflect.ParameterizedType;
import java.util.List;

import org.hibernate.Criteria;
import org.hibernate.Session;
import org.hibernate.Transaction;
import org.hibernate.criterion.Restrictions;

import br.com.diego.agenda.util.HibernateUtil;

public class GenericDAO<Entity> {

	private Class<Entity> classes;

	@SuppressWarnings("unchecked")
	public GenericDAO() {
		this.classes = (Class<Entity>) ((ParameterizedType) getClass().getGenericSuperclass())
				.getActualTypeArguments()[0];
	}

	public void save(Entity entity) {
		Session session = HibernateUtil.getBuildSessionFactory().openSession();
		Transaction transaction = null;

		try {
			transaction = session.beginTransaction();
			session.save(entity);
			transaction.commit();
		} catch (RuntimeException error) {
			if (transaction != null) {
				transaction.rollback();
			}
			throw error;
		} finally {
			session.close();
		}
	}

	@SuppressWarnings("unchecked")
	public List<Entity> list() {
		Session session = HibernateUtil.getBuildSessionFactory().openSession();
		try {

			Criteria query = session.createCriteria(classes);
			List<Entity> result = query.list();
			return result;

		} catch (RuntimeException error) {
			throw error;
		} finally {
			session.close();
		}

	}

	@SuppressWarnings("unchecked")
	public Entity search(Long code) {
		Session session = HibernateUtil.getBuildSessionFactory().openSession();
		try {

			Criteria query = session.createCriteria(classes);
			query.add(Restrictions.idEq(code));

			Entity result = (Entity) query.uniqueResult();
			return result;

		} catch (RuntimeException error) {
			throw error;
		} finally {
			session.close();
		}

	}

	public void delete(Entity entity) {
		Session session = HibernateUtil.getBuildSessionFactory().openSession();
		Transaction transaction = null;

		try {
			transaction = session.beginTransaction();
			session.delete(entity);
			transaction.commit();
		} catch (RuntimeException error) {
			if (transaction != null) {
				transaction.rollback();
			}
			throw error;
		} finally {
			session.close();
		}
	}

	public void update(Entity entity) {
		Session session = HibernateUtil.getBuildSessionFactory().openSession();
		Transaction transaction = null;

		try {
			transaction = session.beginTransaction();
			session.update(entity);
			transaction.commit();
		} catch (RuntimeException error) {
			if (transaction != null) {
				transaction.rollback();
			}
			throw error;
		} finally {
			session.close();
		}
	}

	public void merge(Entity entity) {
		Session session = HibernateUtil.getBuildSessionFactory().openSession();
		Transaction transaction = null;

		try {
			transaction = session.beginTransaction();
			session.merge(entity);
			transaction.commit();
		} catch (RuntimeException error) {
			if (transaction != null) {
				transaction.rollback();
			}
			throw error;
		} finally {
			session.close();
		}
	}

}
